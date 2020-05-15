# UciRod - CI/CD - Jenkins  - ip: 127.0.0.1 - www.ucirod.jenkins.com mapped in C:\Windows\System32\drivers\etc\hosts
http://www.ucirod.jenkins.com:8082 | user: ucirod, password: ucirod

# Inventapp - DEV - ip: 127.0.0.1 - IIS - www.ucirod.inventapp-dev.com mapped in C:\Windows\System32\drivers\etc\hosts
http://www.ucirod.inventapp-dev.com:8080
http://www.ucirod.inventapp-dev.com:8080/WebApi
http://www.ucirod.inventapp-dev.com:8080/WebApi/swagger
http://www.ucirod.inventapp-dev.com:8080/WebApi/hangfire

# Inventapp - TEST - ip: 152.171.94.90 (external public ip) - IIS - 152.171.94.90 mapped to 192.168.0.239 (internal private ip)
http://152.171.94.90:8080
http://152.171.94.90:8080/WebApi
http://152.171.94.90:8080/WebApi/swagger
http://152.171.94.90:8080/WebApi/hangfire

{username: "urodriguez-admin", password: "admin"}

{
  "code": "P00",
  "name": "local_name00",
  "category": "C",
  "price": 13
}

## TODO list
* check invalid user on login
* implement unityOfWork
* implement caching
* delete old client application
* create V2 client application (using ngx-admin https://akveo.github.io/nebular)
  * user login
  * products module
  * users module
* expose client application to external (no localhost) URL via public ip for TEST env
* Reporting: integration - using hangfire recurrent job
* unit test
* Automation: API test
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
* use Docker
* use PusherServer/WebSockets to notify UI on server changes
* deploy app to cloud - PROD env

## DONE list
* automapper
* move tables to own db
* refector repositories to use no hardcoded querys
* implement dbConectionFactory 
* log database queries
* spread log db queries over all repo methods
* remove LogMessage object in Log method for LogService, idem LogLevel
* formatter for generated sql queries to avoid copy paste query parameter
* use swagger
* gitignore logs
* apply internal on classes when is necessary
* improve logs history with more files
* use correlation id on logs
* try-catch wrapping in controllers
* use local IIS
* use Jenkins
* implement authentication use JWT to authenticate
* add auth to swagger
* move logging service to micro-service in net core
* use settings for Test ENV
    -> app settings
    -> connection string
        -> create new databases
* implement basic auditing
* integrate infrastructure in one web site
* implement business validator for Create
* implement require attribute in order to validate require fields on domain
* implement business validator for Update
* integrate all predicates (basic + group) in one (Composite Pattern) for Repository.Get
* implement factory for all predicates (basic + group)
* extend transformation process to dapper predicates to multiple levels
* move infrastructure interface to Domain Layer
* implement Utils to automate class name and class method in log
* adapt to 'auditV2'
* implement await/async without Dapper (current using version is Dapper.Extensions and it is coupled with MiniProfiler.Integration that not support async) => use EF -> REMOVED
* preparate to new isolated token service
* if (!_roleService.LoggedUserIsAdmin()) throw new UnauthorizedAccessException();
* test user readOnly
* create Execute on ApplicationService and handle all exceptions there
* centralize RestSharp into SharedProject
* create user sending confirm email
* create injectable ApplicationSettingsService at crosscutting layer and replace all ConfigurationManager.AppSettings["Environment"]
* autogenerate password and allow user to change it after first login
* catch and process new Authentication response
* get exp token from AppSettings
* refactor Infrastructure services and refecerences
* send credencials to external Infrastructure service
* jenkins - deploy test + test
* update scripts to be idempotent
* Jenkins: deploy assets folder
* Swagger: check error "schemaValidationMessages":[{"level":"error","message" ...
* Jenkins: script to update password on IIS user
* avoid show internal message errors, only log them. Show reference Id
* handle and send custom correlation
* log locally Correlation & LoggingDb
* implement process to delete old logs from file system
* update URLs after versioning
* Reporting: integration - using rest request
* Reporting: integration - using rest request + send by email
* integrate with new auditing
* enqueue failed data
* dequeue failed data and resend
* Queueing: use dependecy resolver
* Jenkins: add sql script to build process
* deploy app to cloud - PROD env
* expose swagger to external (no localhost) URL via public ip for TEST env
* organize readme by ENV


## Angular architercure styleguide
https://angular.io/guide/styleguide

## Development server
Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files.

## Code scaffolding
Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

## Build
Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory. Use the `--prod` flag for a production build.