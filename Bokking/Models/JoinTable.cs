using Bokking.Models;


namespace Bokking.Models
{
    public class JoinTable
    { 
        public UserLogin  userLogin {get; set; }   

        public Customer  customer {get; set; }

        public Reservation  reservation {get; set; }

        public Room  room {get; set; }  

        public Hotel  hotel {get; set; }  
        
        public Service  service {get; set; }


    }
}
