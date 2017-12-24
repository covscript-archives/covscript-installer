#!/bin/bash
function start ()
{
    cd $1
    $2 &
    cd ..
}
mkdir -p build-cache
cd build-cache
git_repo=https://github.com/covscript
git clone $git_repo/covscript
git clone $git_repo/covscript-regex
git clone $git_repo/covscript-darwin
git clone $git_repo/covscript-sqlite
git clone $git_repo/covscript-network
cp -rf covscript/include covscript-regex/
cp -rf covscript/include covscript-darwin/
cp -rf covscript/include covscript-sqlite/
cp -rf covscript/include covscript-network/
start covscript './make.sh'
start covscript-regex './make.sh'
start covscript-darwin './make.sh'
start covscript-sqlite './make.sh'
start covscript-network './make.sh'
wait