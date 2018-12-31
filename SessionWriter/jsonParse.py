import sys
import json
import re
from datetime import datetime

sessionStart = '{"RecordingDate":'
applyDateTime = datetime.now().strftime("%Y-%m-%d %H:%M:%S")
recordingStart = ',"RecordingSession":['
sessionEnd = "]}"

longStart = sessionStart + '"' + applyDateTime + '"' + recordingStart

beginningComma = '\A,'
matchQuote = ',}}"'
matchBrackets = ',}}'
matchTrue = 'True'
matchFalse = 'False'

replaceQuote = '}},"'
replaceBrackets = '}}'
replaceTrue = 'true'
replaceFalse = 'false'

searches = [beginningComma, matchQuote, matchBrackets, matchTrue, matchFalse]    
#finds = []
#searchResults = []



replacements = [longStart, replaceQuote, replaceBrackets, replaceTrue, replaceFalse]

with open('../../../SessionWriter/data/Vitruvius.txt') as f:
    bodytext = f.read()

#for search in searches:
#    finds.append(re.search(search, bodytext))
    
#for find in finds:
#    searchResults.append(re.compile(find[0]))


commaSearch = re.compile(beginningComma)
quoteSearch = re.compile(matchQuote)
bracketSearch = re.compile(matchBrackets)
trueSearch = re.compile(matchTrue)
falseSearch = re.compile(matchFalse)


finalText = commaSearch.sub(longStart, bodytext)
finalText = quoteSearch.sub(replaceQuote, finalText)
finalText = bracketSearch.sub(replaceBrackets, finalText)
finalText = trueSearch.sub(replaceTrue, finalText)
finalText = falseSearch.sub(replaceFalse, finalText)
finalText = finalText + sessionEnd

with open('../../../SessionWriter/data/longVit.txt', 'w') as f:
    f.write(finalText)
    
vitInMemJson = json.loads(finalText)

with open('../../../SessionWriter/data/vitConvert.json', 'w') as f:
    f.write(finalText)
    
vitRecordSession = vitInMemJson['RecordingDate']
#vitFrames = vitInMemJson['RecordingSession']

print(vitRecordSession)
