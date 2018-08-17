rm -rf ./build
mkdir -p build/bin
cd build-cache
cp cspkg/cspkg ../build/bin/
cp -rf covscript/build ..
cp -rf covscript-regex/build ..
cp -rf covscript-darwin/build ..
cp -rf covscript-sqlite/build ..
cp -rf covscript-network/build ..
cp -rf covscript-streams/build ..
cp -rf covscript-imgui/build ..