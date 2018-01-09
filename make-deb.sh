#!/bin/bash
if [ "$#" != "3" ]; then
    echo "Wrong size of arguments."
    exit
fi
mkdir -p deb-src
cd deb-src
mkdir -p ./$2/DEBIAN
mkdir -p ./$2/etc/profile.d
mkdir -p ./$2/$1/share/covscript
cp -r ../build/bin ./$2/$1/share/covscript
cp -r ../build/imports ./$2/$1/share/covscript
SIZE=$(du -s ../build | grep -o -E "[0-9]+")
echo "#!/bin/bash">>./$2/etc/profile.d/covscript-init.sh
echo "export PATH=\"\$PATH:$1/share/covscript/bin\"">>./$2/etc/profile.d/covscript-init.sh
echo "export COVSCRIPT_HOME=\"$1/share/covscript\"">>./$2/etc/profile.d/covscript-init.sh
echo "export CS_IMPORT_PATH=\"$1/share/covscript/imports\"">>./$2/etc/profile.d/covscript-init.sh
echo "Package: covscript">>./$2/DEBIAN/control
echo "Version: $3">>./$2/DEBIAN/control
echo "Section: utils">>./$2/DEBIAN/control
echo "Priority: optional">>./$2/DEBIAN/control
echo "Architecture: $2">>./$2/DEBIAN/control
echo "Installed-Size: $SIZE">>./$2/DEBIAN/control
echo "Maintainer: Michael Lee <mikecovlee@163.com>">>./$2/DEBIAN/control
echo "Description: Covariant Script Programming Language">>./$2/DEBIAN/control
echo >>./$2/DEBIAN/control
echo "#!/bin/bash">>./$2/DEBIAN/postinst
echo "echo \"You must run \\\"source /etc/profile.d/covscript-init.sh\\\" or restart your computer to activate Covariant Script.\"">>./$2/DEBIAN/postinst
echo "echo \"Please visit <http://covscript.org/> for more information.\"">>./$2/DEBIAN/postinst
chmod -R 755 ./$2/DEBIAN
dpkg-deb -b $2 covscript-$3-$2.deb
rm -rf $2