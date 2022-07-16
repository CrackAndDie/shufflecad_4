#!/bin/bash
sudo ps -ef | grep pyNameHere | grep -v grep | awk '{print $2}' | sudo xargs -r kill -9