using UnityEngine;

//scene managers handle the initialization of a specific scene
//
//Initializes all our scenes First Level Controllers
//More controllers may be initialized later when we load a model that uses other components.
//This means the logic flow is First Level controller loads in model, that model may have componenets that are managed by sub controllers (hotel -> room -> placable)
//Each one of these sub controllers are responsible for the connection between their level of view and model.  Models since they have no logic, cannot trigger view refresh
//so we leave that to the controllers since a controller is the only thing that can change the view anyway. Not all controllers need a view if there are no view elements.
//Simularly, they dont all need a model if there is no data elements.  In this case a controller can be seen as a normal logic script.  
//views attached to the game object
//
//          Scene Manager        
//            ↓ x n
// model ⇋ controller ⇋ view
//            ↓ x n
// model ⇋ controller ⇋ view
//              ↓
//         logic script

// When we load a controller, does the controller know what to initialize or does the thing initializing it .. it should be what inits it...which means we pass in info about how to load its model

namespace Managers.SceneManagers
{
    public class SceneManagerBase : MonoBehaviour
    {
        public Canvas ScenesMainCanvas;
        
        public virtual void Init()
        {
        }
    }
}