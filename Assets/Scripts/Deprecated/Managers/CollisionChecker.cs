using UnityEngine;
using System;

namespace DoubleTrouble.Managers
{
    public class CollisionChecker : MonoBehaviour
    {
        public static event Action<GameObject> OnBothPlayersColliding;

        private PlayersMovement player1;
        private PlayersMovement player2;
        
        private void Start()
        {
            
        }

       
    }

}

