from confluent_kafka import Producer


topic = "raw_reports"

conf = {'bootstrap.servers': 'localhost:9092',
        'client.id': "dsa"}

producer = Producer(conf)


def sent_to_kafka(massage):
    try:
        producer.produce(topic, key="key", value=massage)
        producer.poll(1)
        print("report sent to kafka")
    except:
        print("error accreed when sending data to kafka  ")



