from confluent_kafka import Producer
import os

kafka_server = os.environ.get("KAFKA_BOOTSTRAP_SERVER")
topic = os.getenv("KAFKA_RAW_REPORTS_TOPIC","raw_reports")
clientId = os.getenv("KAFKA_CLIENT_ID")

print(f"Connecting to Kafka at: {kafka_server}")




conf = {'bootstrap.servers': kafka_server,
        'client.id': clientId}

producer = Producer(conf)


def sent_to_kafka(massage):
    try:
        producer.produce(topic, key="key", value=massage)
        producer.poll(1)
        print("report sent to kafka")
    except:
        print("error accreed when sending data to kafka  ")



