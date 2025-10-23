from transformers import pipeline
import gradio as gr

model = pipeline("sentiment-analysis")

def sentiment_analysis(text):
    result = model(text)[0]
    return {
        "label": result['label'],
        "score": result['score']
    }


iface = gr.Interface(fn=sentiment_analysis, inputs=gr.Textbox(lines=2, placeholder="Enter your text here..."), outputs=gr.JSON())

iface.launch()