# TTS (Test Speech)
# pip install gTTS
# pip install playsound

from gtts import gTTS
from playsound import playsound

text = '안녕하세요, 곽지은입니다.'

tts = gTTS(text=text, lang='ko', slow=False)
tts.save('./studyPython/hi.mp3')
print('완료!')
playsound('./studyPython/output/hi.mp3')
print('음성출력 완료!')
