#!/bin/sh
# Android

# German
mono ../Vernacular/Vernacular.Tool/bin/Debug/Vernacular.exe \
  --input=./Translations/de.po \
  --generator=android \
  --output=./WF.Player.Droid/Resources/values-de/vernacular_strings.xml \
  --android-input-strings-xml=./WF.Player.Droid/Resources/values/strings.xml \
  --android-output-strings-xml=./WF.Player.Droid/Resources/values-de/strings.xml \
  --verbose
  
# French
mono ../Vernacular/Vernacular.Tool/bin/Debug/Vernacular.exe \
  --input=./Translations/fr.po \
  --generator=android \
  --output=./WF.Player.Droid/Resources/values-fr/vernacular_strings.xml \
  --android-input-strings-xml=./WF.Player.Droid/Resources/values/strings.xml \
  --android-output-strings-xml=./WF.Player.Droid/Resources/values-fr/strings.xml \
  --verbose
  
# Finnish
mono ../Vernacular/Vernacular.Tool/bin/Debug/Vernacular.exe \
  --input=./Translations/fi.po \
  --generator=android \
  --output=./WF.Player.Droid/Resources/values-fi/vernacular_strings.xml \
  --android-input-strings-xml=./WF.Player.Droid/Resources/values/strings.xml \
  --android-output-strings-xml=./WF.Player.Droid/Resources/values-fi/strings.xml \
  --verbose

# iOS

# German
mono ../Vernacular/Vernacular.Tool/bin/Debug/Vernacular.exe \
  --input=./Translations/de.po \
  --generator=ios \
  --output=./WF.Player.iOS/Resources/de.lproj/Localizable.strings \
  --verbose

# French
mono ../Vernacular/Vernacular.Tool/bin/Debug/Vernacular.exe \
  --input=./Translations/fr.po \
  --generator=ios \
  --output=./WF.Player.iOS/Resources/fr.lproj/Localizable.strings \
  --verbose

# Finnish
mono ../Vernacular/Vernacular.Tool/bin/Debug/Vernacular.exe \
  --input=./Translations/fi.po \
  --generator=ios \
  --output=./WF.Player.iOS/Resources/fi.lproj/Localizable.strings \
  --verbose
