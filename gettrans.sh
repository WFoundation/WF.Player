#!/bin/sh
mono ../Vernacular/Vernacular.Tool/bin/Debug/Vernacular.exe \
  --input=./WF.Player.Droid/Resources/values/strings.xml \
  --input=./WF.Player.Droid/Resources/values/array.xml \
  --input=./WF.Player.Droid/bin/Debug/ \
  --input=./WF.Player.iOS/bin/iPhone/Debug/ \
  --input=./WF.Player.iOS/Settings.bundle/ \
  --source-root=./ \
  --output=./WF.Player.pot \
  --generator=po \
  --log \
  --verbose
  
# Create Vernacular file for Android
mono ../Vernacular/Vernacular.Tool/bin/Debug/Vernacular.exe \
  --input=./WF.Player.Droid/Resources/values/strings.xml \
  --input=./WF.Player.Droid/Resources/values/array.xml \
  --input=./WF.Player.Droid/bin/Debug/ \
  --source-root=./  \
  --output=./WF.Player.Droid/Resources/values/vernacular_strings.xml \
  --generator=android \
  --log