namespace CoreLibrary
{
    public class KafkaOptions
    {
        public static string Kafka = "Kafka";
        public string BootstrapUrl { get; set; } = string.Empty;
        public string Topic { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string SaslMechanism { get; set; } = string.Empty;
        public Confluent.Kafka.SaslMechanism SaslMechanismEnum
        {
            get
            {
                return SaslMechanism.ToUpper() switch
                {
                    "GSSAPI" => Confluent.Kafka.SaslMechanism.Gssapi,
                    "PLAIN" => Confluent.Kafka.SaslMechanism.Plain,
                    "SCRAM-SHA-256" => Confluent.Kafka.SaslMechanism.ScramSha256,
                    "SCRAM-SHA-512" => Confluent.Kafka.SaslMechanism.ScramSha512,
                    "OAUTHBEARER" => Confluent.Kafka.SaslMechanism.OAuthBearer,
                    _ => Confluent.Kafka.SaslMechanism.Plain
                };
            }
        }
    }
}