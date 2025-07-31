from chatterbot import ChatBot #  bot em si
from chatterbot.trainers import ListTrainer #  treinar o bot com uma lista de frases/respostas.

chatbot = ChatBot('RoboBot') #  Cria uma instância
trainer = ListTrainer(chatbot) # Cria um treinador que usa a lista

# Lista de perguntas e respostas que o bot vai usar para aprender.
# O treinamento é baseado em pares de entrada/saída.
trainer.train(
    ["Oi",
    "Bem-vindo, amigo!"]
)
trainer.train(
    ["Você e um bot?",
    "Não, eu sou um chatbot!"]
)
trainer.train(
    ["O que você faz?",
    "Eu auxilio no treinamento!"]
)

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