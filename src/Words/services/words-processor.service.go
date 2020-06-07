package services

import (
	"log"
	"net/http"
	"os"
	"strings"
	"time"

	"golang.org/x/net/html"
)

// WordsProcessorService structure
type WordsProcessorService struct {
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

	log.Print(wordsURL)
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

	return os.Getenv("SJP_URL") + wordsFilename, nil
}
