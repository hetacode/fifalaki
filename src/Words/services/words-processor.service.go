package services

import (
	"archive/zip"
	"bufio"
	"bytes"
	"fmt"
	"io"
	"log"
	"net/http"
	"os"
	"strings"
	"time"

	"github.com/hetacode/fifalaki/words/db"
	"golang.org/x/net/html"
)

// WordsProcessorService structure
type WordsProcessorService struct {
	DB *db.RedisKeyValueDB
}

// Processing write side logic
// 1. download words list from https://sjp.pl/slownik/growy/ in cron scheduler - on start and then 24h
// 2. unpack zip
// 3. process words - take only 5-7 chars words
// 4. put them to redis
func (s *WordsProcessorService) Processing() {
	log.Printf("Words processing is started on %s", time.Now().Format("Mon Jan _2 15:04:05 2006"))

	wordsURL, err := parseSJPPageAndGetWordsZIPUrl()
	if err != nil {
		log.Print(err)
		return
	}
	file, err := http.Get(wordsURL)
	if err != nil {
		log.Print(err)
		return
	}
	defer file.Body.Close()

	buff := bytes.NewBuffer([]byte{})
	size, err := io.Copy(buff, file.Body)
	if err != nil {
		log.Print(err)
		return
	}

	reader := bytes.NewReader(buff.Bytes())
	z, err := zip.NewReader(reader, size)
	if err != nil {
		log.Print(err)
		return
	}

	var wordsFile *zip.File
	for _, file := range z.File {
		log.Printf("%s", file.Name)
		if file.Name == os.Getenv("WORDS_FILENAME") {
			wordsFile = file
			break
		}
	}

	if wordsFile == nil {
		log.Print("Cannot fint words file inside zip")
		return
	}

	if err := s.wordsProcessing(wordsFile); err != nil {
		log.Print(err)
	}

}

func (s *WordsProcessorService) wordsProcessing(file *zip.File) error {
	log.Print("Start processing words")
	r, err := file.Open()
	if err != nil {
		return err
	}

	total := 0
	processed := 0
	scan := bufio.NewScanner(r)
	for scan.Scan() {
		text := scan.Text()
		l := len(text)
		total++
		if l >= 5 && l <= 7 {
			processed++
			s.DB.Put(text, time.Now().Format("Mon Jan 2 15:04:05 -0700 MST 2006"))
		}
	}
	log.Printf("End processing words | Total: %d Processed: %d", total, processed)
	return nil
}

func parseSJPPageAndGetWordsZIPUrl() (string, error) {
	page, err := http.Get(os.Getenv("SJP_URL"))
	if err != nil {
		return "", err
	}
	defer page.Body.Close()
	doc, err := html.Parse(page.Body)
	if err != nil {
		return "", err
	}

	wordsFilename := ""
	var f func(*html.Node)
	f = func(n *html.Node) {
		if n.Type == html.ElementNode && n.Data == "a" {
			for _, a := range n.Attr {
				if a.Key == "href" {
					if strings.HasPrefix(a.Val, "sjp-") {
						wordsFilename = a.Val
						return
					}
					log.Println(a.Val)
					break
				}
			}
		}
		for c := n.FirstChild; c != nil; c = c.NextSibling {
			f(c)
		}
	}
	f(doc)
	if wordsFilename == "" {
		return "", fmt.Errorf("Cannot find zip file url")
	}
	return os.Getenv("SJP_URL") + wordsFilename, nil
}
