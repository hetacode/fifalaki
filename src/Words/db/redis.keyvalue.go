package db

import (
	"context"

	"github.com/go-redis/redis/v8"
)

// RedisKeyValueDB key value db struct
type RedisKeyValueDB struct {
	client *redis.Client
}

// NewRedisDB create new instance of redis db
func NewRedisDB(server string, db int) *RedisKeyValueDB {
	client := redis.NewClient(&redis.Options{
		Addr: server,
		DB:   db,
	})

	i := &RedisKeyValueDB{
		client: client,
	}

	return i
}

// Put key value data
func (db *RedisKeyValueDB) Put(key string, value string) error {
	r := db.client.Set(context.TODO(), key, value, 0)
	if r.Err() != nil {
		return r.Err()
	}

	return nil
}

// Get value for given key
func (db *RedisKeyValueDB) Get(key string) (string, error) {
	r := db.client.Get(context.TODO(), key)
	if r.Err() != nil {
		return "", r.Err()
	}

	return r.Result()
}

// GetRandom element from db
func (db *RedisKeyValueDB) GetRandom() (key string, value string, err error) {
	r := db.client.RandomKey(context.TODO())
	if r.Err() != nil {
		return "", "", r.Err()
	}
	v := r.Val()
	k, err := r.Result()

	if err != nil {
		return "", "", err
	}

	return k, v, nil
}
