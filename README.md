# CALC_RPN_API
RPN_API Améliorations :

- Pousser les operation dans une autre class service avec l'injection par interface et pour mettre moins de metier dans un controller  (le passe plat)
Exemple  

        private readonly IRpnService _rpnService;
 public RPNController( IRpnService rpnService)
        {
        
            this.rpnService = rpnService;
        }
        
et l'interface aura les signaturse des operations 
   public interface IRpnService
    {
      // l'ajout  exemple :< bool CreateNewStack();
      // lister les opertaion  
      //....... 
    }
    
    et le service implemetera l'interface 
    - Ajout des logsdans le service   selon si on veut vraiment tout tracer 
 
    public class RpnService : IRpnService
    {
       private readonly ILogger<RPNController> _logger; 
       public RpnService (ILogger<RpnService> logger){
            _logger = logger;
        }
      //  definier les implementations des operations ici  
    }
    
    // IOC 
      ajout dans le startup  
      services.AddScoped<IRpnService, RpnService>(_logger); // avec _logger est un ILogger<Startup>


Ajout de projet de test unitaire 
