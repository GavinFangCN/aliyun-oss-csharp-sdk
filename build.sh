#!/usr/bin/env bash
set -e
basepath=$(cd `dirname $0`; pwd)
artifacts=${basepath}/artifacts

if [[ -d ${artifacts} ]]; then
   rm -rf ${artifacts}
fi

mkdir -p ${artifacts}

project=sdk/aliyun-oss-sdk-dotnet.csproj

dotnet restore $project

dotnet build $project -f netstandard2.0 -c Release -o ${artifacts}/netstandard2.0
dotnet build $project -f net40 -c Release -o ${artifacts}/net40
