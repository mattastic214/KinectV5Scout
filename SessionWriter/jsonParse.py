import sys
import json
import pickle
import re
from datetime import datetime

inputFile = sys.argv[0]
print(sys.argv[0])

textFileExt = '.txt'
jsonFileExt = '.json'
pickleFileExt = ".pickle"


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


replacements = [longStart, replaceQuote, replaceBrackets, replaceTrue, replaceFalse]

with open(inputFile) as f:
    bodytext = f.read()

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

    
vitInMemJson = json.loads(finalText)

extSearch = re.compile(textFileExt)
outputFile = extSearch.sub(jsonFileExt, inputFile)

with open(outputFile, 'w') as f:
    f.write(finalText)

with open("vitruvius.pickle", 'wb') as f:
	pickle.dump(finalText, f)
    
vitRecordSession = vitInMemJson['RecordingDate']
print(vitRecordSession)
