import json

from ProducerServies import KafkaProducer


def main():
    coun = 0
    with open("Data/field_reports.json","r",encoding='UTF-8') as file_data:
        reports = json.load(file_data)
        for report in reports:
           report = json.dumps(report)
           KafkaProducer.sent_to_kafka(report)
           coun+=1
        KafkaProducer.producer.flush()
        print(coun)    


main()