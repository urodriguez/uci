# UciRod - Continous Integration - Jenkins
http://localhost:8082 -> Jenkins (run by cmd: java -jar "C:\Program Files (x86)\Jenkins\jenkins.war" --httpPort=8082)

# UciRod.Inventapp URL -> ENV
http://localhost:8080 -> DEV
http://www.ucirod.inventapp-test.com:8083 -> TEST (IIS Local - overwritten in C:\Windows\System32\drivers\etc\hosts)

# UciRod.Inventapp.WebApi URL -> ENV
http://localhost:8080/WebApi -> DEV
http://www.ucirod.inventapp-test.com:8083/WebApi -> TEST

# UciRod.Inventapp.WebApi - Swagger - URL -> ENV
http://localhost:8080/WebApi/swagger -> DEV
http://www.ucirod.inventapp-test.com:8083/WebApi/swagger -> TEST

# UciRod.Inventapp.WebApi - Hangire - URL -> ENV
http://localhost:8080/WebApi/hangfire -> DEV
http://www.ucirod.inventapp-test.com:8083/WebApi/hangfire -> TEST

{username: "urodriguez-admin", password: "admin"}

{
  "code": "P00",
  "name": "local_name00",
  "category": "C",
  "price": 13
}

## TODO list
* Infrastructe: enqueue failed data
* Infrastructe: dequeue failed data and resend
* expose swagger api url to external (no localhost) URL
* implement caching
* InventApp.Reporting: integration - using hangfire recurrent job 
* Infrastructe: API Gateway
* InventApp: create application (Angular)
  * user login
  * products module
  * users module
* Infrastructure: create application (React)
  * user register -> receive email
  * user login
  * user see list available infrastructure services
  * user select infrastructure services
  * user pay -> credit card transactions?
  * logs module
* Infrastructure.Authentication: implement refresh token
* InventApp: implement unityOfWork
* API test & unit test
* create index in db tables
* create all necessary elements to create registration
  * CRUD invention type
  * CRUD invention
  * CRUD sponsor
  * CRUD sponsor/invention
  * CRUD site 
    * site1 -> manually
    * site2 -> excel (using Excel library)
    * site3 -> rest GET
    * site4 -> soap
    * site5 -> rest POST (notification on new one + recurrent db updates)
  * CRUD innovator
  * CRUD customer
  * CRUD registration reason
  * CRUD registration status
* implement integration events with NServiceBus
* create script to configure sites on IIS
* use TeamCity
* use Docker
* use PusherServer/WebSockets to notify UI on server changes
* deploy app to cloud 

## DONE list
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
* InventApp-Infrastructure: move auth service to Infrastructure solution -> DONE
* Infrastructure.Authentication: deploy -> DONE
* InventApp: centralize RestSharp into SharedProject -> DONE
* Infrastructure.Mailing: implement -> DONE
* InventApp: create user sending confirm email -> DONE
* InventApp: create injectable ApplicationSettingsService at crosscutting layer and replace all ConfigurationManager.AppSettings["Environment"] -> DONE
* InventApp: autogenerate password and allow user to change it after first login -> DONE
* Infrastructure: differentiate between invalid token or expired token exception (catching corrrect exception) -> DONE
* InventApp: catch and process new Authentication response -> DONE
* Infrastructure.Authentication: reduce exp token to 1h -> DONE
* InventApp: get exp token from AppSettings -> DONE
* InventApp: refactor Infrastructure services and refecerences -> DONE
* Infrastructure: use credencials validation in all products -> DONE
* InventApp: send credencials to external Infrastructure service -> DONE
* Infrastructure: jenkins - stop app pool before build -> DONE
* Infrastructure: create BaseApiController and do credentials validation there -> DONE
* InventApp: jenkins - deploy test + test -> DONE
* InventApp/Infrastructure: update scripts to be idempotent -> DONE
* InventApp.Test: jenkins - deploy assets folder -> DONE
* Infrastructure.Test: jenkins - full deploy + improve powershell scripts -> DONE
* Infrastructure.Auditing: handling exceptions-> DONE
* InventApp.Test: swagger - check error "schemaValidationMessages":[{"level":"error","message" ... -> DONE
* UciRod: jenkins - script to update password on IIS user -> DONE
* InventApp: avoid show internal message errors, only log them. Show reference Id -> DONE
* Infrastructure: avoid show internal message errors, only log them. Show reference Id -> DONE
* Infrastructure: remove correlations endpoint + make it optional on dto and string -> DONE
* InventApp: handle and send custom correlation -> DONE
* InventAppInfrastructure: log locally Correlation & LoggingDb -> DONE
* Infrastructure: log locally Correlation & LoggingDb -> DONE
* Infrastructure.Logging: implement process to delete old logs (database and file system) -> DONE
* InventApp: implement process to delete old logs from file system -> DONE
* Infrastructure: versioning all projects -> DONE
* InventApp: update URLs after versioning -> DONE
* Infrastructure.Reporting: implement -> DONE
* InventApp.Reporting: integration - using rest request -> DONE
* Infrastructure.Mailing: allow attachments -> DONE
* InventApp.Reporting: integration - using rest request + send by email -> DONE
* Infrastructure.Auditing: implement complex auditing - objects with nested objects -> DONE
* Infrastructure.Auditing: implement complex auditing - objects with nested arrays of objects -> DONE
* InventApp: integrate with new auditing -> DONE
* Infrastructure.Auditing: test on TEST env + fix bugs -> DONE
* Infrastructure.Auditing: feature to add/remove elements on object/array -> DONE
* InventApp: enqueue failed data -> DONE
* InventApp: dequeue failed data and resend -> DONE

## Angular architercure styleguide

https://angular.io/guide/styleguide

## Development server

Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files.

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

## Build

Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory. Use the `--prod` flag for a production build.