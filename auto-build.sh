#!/bin/bash
bash ./build.sh
bash ./install.sh
version_prefix=`echo "runtime.info()" | ./build/bin/cs_repl --silent | grep "Version: " | awk '{print $2}'`
version_suffix=`echo "runtime.info()" | ./build/bin/cs_repl --silent | grep "Version: " | awk '{print $NF}'`
architecture=`dpkg --print-architecture`
bash ./make-deb.sh /usr $architecture ${version_prefix}"."${version_suffix}