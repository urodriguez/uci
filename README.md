# UciRod - Continous Integration - Jenkins
http://localhost:8082 -> Jenkins (run by cmd: java -jar "C:\Program Files (x86)\Jenkins\jenkins.war" --httpPort=8082)

# UciRod.Inventapp URL -> ENV
http://localhost:8080 -> DEV

http://www.ucirod.inventapp-test.com:8083 -> TEST (IIS Local - overwritten in C:\Windows\System32\drivers\etc\hosts)

# UciRod.Inventapp.WebApi URL -> ENV
http://localhost:8080/WebApi -> DEV

http://www.ucirod.inventapp-test.com:8083/WebApi -> TEST

# UciRod.Inventapp.WebApi - Swagger - URL -> ENV
http://localhost:8080/WebApi/Swagger -> DEV

http://www.ucirod.inventapp-test.com:8083/WebApi/Swagger -> TEST

{username: "urodriguez-admin", password: "admin"}

{
  "code": "P00",
  "name": "local_name00",
  "category": "C",
  "price": 13
}

## TODO list - backend
* automapper -> DONE
* move tables to own db -> DONE
* refector repositories to use no hardcoded querys -> DONE
* implement dbConectionFactory -> DONE 
* log database queries -> DONE
* spread log db queries over all repo methods -> DONE
* remove LogMessage object in Log method for LogService, idem LogLevel -> DONE
* formatter for generated sql queries to avoid copy paste query parameter -> DONE
* use swagger -> DONE
* gitignore logs -> DONE
* apply internal on classes when is necessary -> DONE
* improve logs history with more files -> DONE
* use correlation id on logs -> DONE
* try-catch wrapping in controllers -> DONE
* use local IIS -> DONE
* use Jenkins -> DONE
* implement authentication use JWT to authenticate -> DONE
* add auth to swagger -> DONE
* move logging service to micro-service in net core -> DONE
* use settings for Test ENV
    -> app settings -> DONE
    -> connection string -> DONE
        -> create new databases -> DONE
* implement basic auditing -> DONE
* integrate infrastructure in one web site -> DONE
* implement business validator for Create -> DONE
* implement require attribute in order to validate require fields on domain -> DONE
* implement business validator for Update -> DONE
* integrate all predicates (basic + group) in one (Composite Pattern) for Repository.Get -> DONE
* implement factory for all predicates (basic + group) -> DONE
* extend transformation process to dapper predicates to multiple levels -> DONE
* move infrastructure interface to Domain Layer -> DONE
* implement Utils to automate class name and class method in log -> DONE
* Auditing: 'auditV2'move logic to process old entity (storing the current state) -> DONE
* InventApp: adapt to 'auditV2' -> DONE
* implement await/async without Dapper (current using version is Dapper.Extensions and it is coupled with MiniProfiler.Integration that not support async) => use EF -> REMOVED
* InventApp: preparate to new isolated token service -> DONE
* InventApp: if (!_roleService.LoggedUserIsAdmin()) throw new UnauthorizedAccessException(); <- move to CrudService? -> DONE
* InventApp: test user readOnly -> DONE
* InventApp: create Execute on ApplicationService and handle all exceptions there -> DONE
* InventApp-Infrastructe: move auth service to Infrastructure solution -> DONE
* Infrastructe.Authentication: deploy -> DONE
* InventApp: centralize RestSharp into SharedProject -> DONE
* Infrastructe.Mailing: implement -> DONE
* InventApp: create user sending confirm email -> DONE
* InventApp: create injectable ApplicationSettingsService at crosscutting layer and replace all ConfigurationManager.AppSettings["Environment"] -> DONE
* InventApp: autogenerate password and allow user to change it after first login -> DONE

* InventApp: diff between invalid token or expired token exception
* InventApp: avoid show internal message errors, only log them. Show reference Id
* Infrastructe: versioning all projects + InventApp: update URLs
* Infrastructe.Logging: implement process to delete old logs (one mounth) in database (maybe directly in LogService) - use hangfire with recurrent job?
* Infrastructe: create application
  * user register -> receive email
  * user login
  * user see list available infrastructure services
  * user select infrastructure services
  * user pay -> credit card transactions?
* Infrastructe.Authentication: implement refresh token
* InventApp: implement unityOfWork
* API test & unit test
* Auditing: implement complex auditing (objects with nested objects) 
* InventApp: implement retry queue system to avoid lost data sent to micro-services if connection fails
* create index in db tables
* create all necessary elements to create registration
  * CRUD panel type
  * CRUD panel
  * CRUD lab
  * CRUD lab/panel
  * CRUD site 
    * site1 -> manually
    * site2 -> excel (using Excel library)
    * site3 -> rest GET
    * site4 -> soap
    * site5 -> rest POST (notification on new one + recurrent db updates)
  * CRUD innovator
  * CRUD customer
  * CRUD test reason
  * CRUD status
* implement reporting
* implement caching
* implement integration events with NServiceBus
* create application to show logs
* create script to configure sites on IIS
* use TeamCity
* use Docker
* use PusherServer/WebSockets to notify UI on server changes
* expose swagger api url to external (no localhost) URL 
* deploy app to cloud 

## Angular architercure styleguide

https://angular.io/guide/styleguide

## Development server

Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files.

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

## Build

Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory. Use the `--prod` flag for a production build.