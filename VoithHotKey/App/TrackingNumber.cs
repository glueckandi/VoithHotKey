using System;

namespace VoithHotKey.App
{
    struct TrackingNumber : IEquatable<TrackingNumber> {

        public string Id { get; }
        
        public TrackingNumber(string nr) {
            Id = nr.Trim();
        }

        public override bool Equals(object obj) {
            
            if (!(obj is TrackingNumber)) {
                return false;
            }
            if ((TrackingNumber)obj == this) {
                return true;
            }

            return Id.Equals(((TrackingNumber)obj).Id);
        }

        public override int GetHashCode() {
            return Id.GetHashCode();
        }

        public override string ToString() {
            return Id;
        }

        public bool Equals(TrackingNumber other) {
            return Id.Equals(other.Id);
        }

        public static bool operator ==(TrackingNumber a, TrackingNumber b) {
            return a.Id.Equals(b.Id);
        }

        public static bool operator !=(TrackingNumber a, TrackingNumber b) {
            return !(a.Id.Equals(b.Id));
        }

    }
}
