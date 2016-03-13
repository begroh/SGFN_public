#!/usr/bin/env bash

# All C# files
files=$(find ./Assets/Scripts/ -name *.cs)

# Don't retain backups.
# Allman style formatting/indenting. Broken brackets.
# Four space tabs.
# Insert empty lines around unrelated blocks, labels, classes, etc.
# Remove padding around parenthesis.
# Add padding around operators.
# Insert space padding after paren headers (e.g. 'if', 'for').
# Add brackets to unbracketed one line conditional statements.
# Convert tabs to the appropriate number of spaces.
# Force use of Linux line end style.
echo $files | xargs astyle -n -A1 -s4 --break-blocks --unpad-paren --pad-oper --pad-header --add-brackets --convert-tabs --lineend=linux

# Condense multiple blank lines -> one blank line.
echo $files | xargs sed -i '/^$/N;/^\n$/D'

# Remove, you know, public bool ________________;.
echo $files | xargs sed -E -i '/public bool[[:space:]]+_+;/D'

# Remove BOM marks
echo $files | xargs sed -E -i $'1s/^\xEF\xBB\xBF//'
