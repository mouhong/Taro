using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Tryout.Domain.Events;

namespace Taro.Tryout.Domain
{
    public class Customer
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int AwardPoints { get; private set; }

        public void IncreaseAwardPoint(int points)
        {
            if (points <= 0)
                throw new ArgumentException("Points should be greater than 0.");

            AwardPoints += points;

            Console.WriteLine("[Domain] Award points + " + points);

            DomainEvent.Apply(new AwardPointsChanged(this, points));
        }
    }
}
