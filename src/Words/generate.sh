#!/bin/sh
rm -rf gen
mkdir gen
protoc -I ../ --go_out=plugins=grpc:./gen/ ../ProtoContracts/WordsServices.proto