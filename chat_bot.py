from chatterbot import ChatBot #  bot em si
from chatterbot.trainers import ListTrainer #  treinar o bot com uma lista de frases/respostas.

chatbot = ChatBot('RoboBot') #  Cria uma instância

# Lista de perguntas e respostas que o bot vai usar para aprender.
# O treinamento é baseado em pares de entrada/saída.
training_data = [
    "Oi ?",
    "Olá redirecionaremos a um de nossos especialistas"
    "Oi, como vai?",
    "Estou bem, obrigado por perguntar",
    "Qual o seu nome?",
    "Sou Robobot, criado para lhe auxiliar",
    "preciso de ajuda",
    "Diga o que deseja....",
    "Lhe atenderemos assim que possivel"
]

trainer = ListTrainer(chatbot) # Cria um treinador que usa a lista
trainer.train(training_data) # para ensinar ao bot as respostas associadas às frases

#Loop de atendimento
print("Iniciando chat com um de nossos atendentes (sair para encerrar) :")
while True:
    user_input = input("Eu: ")
    if user_input.lower() == 'sair':
        break
    response = chatbot.get_response(user_input)
    print(f"RoboBot: {response}")

# infos
#O ChatterBot aprende por similaridade de texto. Ele busca uma entrada parecida no que foi treinado e retorna a resposta associada. Quanto maior e mais bem estruturado o conjunto de dados, melhores serão as respostas.