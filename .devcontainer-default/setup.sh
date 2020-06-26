echo -n "Setting up..." > /home/vsonline/workspace/ai-fundamentals/"WAIT"
cd /usr/bin
mv /home/vsonline/workspace/ai-fundamentals/"WAIT" /home/vsonline/workspace/ai-fundamentals/"WAIT."
curl https://bootstrap.pypa.io/get-pip.py -o get-pip.py
mv /home/vsonline/workspace/ai-fundamentals/"WAIT." /home/vsonline/workspace/ai-fundamentals/"WAIT.."
python3 get-pip.py
mv /home/vsonline/workspace/ai-fundamentals/"WAIT.." /home/vsonline/workspace/ai-fundamentals/"WAIT..."
pip install jupyter matplotlib pillow requests azure-cognitiveservices-vision-computervision azure-cognitiveservices-vision-customvision azure-cognitiveservices-vision-face azure-cognitiveservices-language-textanalytics azure.cognitiveservices.speech azureml-sdk
mv /home/vsonline/workspace/ai-fundamentals/"WAIT..." /home/vsonline/workspace/ai-fundamentals/"z REFRESH NOW!"