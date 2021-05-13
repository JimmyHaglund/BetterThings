$scriptDir = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent
& docker run --rm -v $scriptDir/AfSearch:/app af-search python AfSearch.py