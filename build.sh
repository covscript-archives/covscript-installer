#!/bin/bash
PREFIX="/usr"
if [ "$#" = "1" ]; then
    PREFIX=$1
fi
function start ()
{
    cd $1
    $2
    cd ..
}
mkdir -p build-cache
cd build-cache
git_repo=https://github.com/covscript
function fetch_git ()
{
    if [ ! -d "$1" ]; then
        git clone $git_repo/$1
    else
        cd $1
        git fetch
        git pull
        git clean -dfx
        cd ..
    fi
}
fetch_git covscript &
fetch_git covscript-regex &
fetch_git covscript-darwin &
fetch_git covscript-sqlite &
fetch_git covscript-network &
wait
cp -rf covscript/include covscript-regex/ &
cp -rf covscript/include covscript-darwin/ &
cp -rf covscript/include covscript-sqlite/ &
cp -rf covscript/include covscript-network/ &
wait
start covscript "bash ./make.sh $PREFIX" &
start covscript-regex "sh ./make.sh" &
start covscript-darwin "sh ./make.sh" &
start covscript-sqlite "sh ./make.sh" &
start covscript-network "sh ./make.sh" &
wait