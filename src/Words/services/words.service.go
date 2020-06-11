package services

import (
	"context"

	"github.com/hetacode/fifalaki/words/db"
	proto "github.com/hetacode/fifalaki/words/gen/ProtoContracts"
)

type WordsServices struct {
	DB *db.RedisKeyValueDB
}

func (s *WordsServices) GetWordsPackage(ctx context.Context, req *proto.GetWordsPackageReq) (*proto.GetWordsPackageRes, error) {
	return nil, nil
}
