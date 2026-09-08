import json

from ProducerServies import KafkaProducer


def main():
    with open("Data/field_reports.json","r",encoding='UTF-8') as file_data:
        reports = json.load(file_data)
        for report in reports:
           report = str( report)
           KafkaProducer.sent_to_kafka(report)

        KafkaProducer.producer.flush()
            


main()