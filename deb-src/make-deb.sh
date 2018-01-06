mkdir -p ./amd64
cp -r ./DEBIAN ./amd64
mkdir -p ./amd64/usr/bin
cp -r ../build/bin ./amd64/usr
mkdir -p ./amd64/etc/covscript/imports
cp -r ../build/imports ./amd64/etc/covscript
chmod -R 755 ./amd64/DEBIAN
dpkg-deb -b amd64 covscript-1.2.1-amd64.deb
rm -rf amd64