﻿namespace CoreLibrary
{
    public class KafkaOptions
    {
        public static string Kafka = "Kafka";
        public string BootstrapUrl { get; set; } = string.Empty;
        public string Topic { get; set; } = string.Empty;

    }
}