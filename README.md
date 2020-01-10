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

* implement factory for all predicates (basic + group)

* extend transformation process to dapper predicates to multiple leves
* implement Utils to automate class name and class method in log
    {
      var st = new StackTrace();
      var sf = st.GetFrame(0);

      var currentMethodName = sf.GetMethod();
      var currentMethodName2 = MethodBase.GetCurrentMethod();
    
      _logService.LogInfoMessage($"{GetType().Name}.{currentMethodName} | Element in database obtained, checking if it is not null", MessageType.Query);
    }
* implement complex auditing (objects with nested objects) 
* implement mailing
* implement reporting
* implement retry queue system to avoid lost data sent to micro-services if connection fails
* implement caching
* implement refresh token
* move auth service to infra solution
* use hangfire
* implement integration events with NServiceBus
* implement at least one soap service
* implement import using Excel library
* create application to show logs
* implement process to delete old logs (one mounth) in database (maybe directly in LogService) - or process to migrate to a new table (example: Audit.ddmmyyyy)
* API test & unit test
* implement await/async without Dapper (current using version is Dapper.Extensions and it is coupled with MiniProfiler.Integration that not support async) => use EF
* implement unityOfWork
* use TeamCity
* use Docker
* use PusherServer to notify UI on server changes
* deploy app to cloud 

## Angular architercure styleguide

https://angular.io/guide/styleguide

## Development server

Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files.

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

## Build

Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory. Use the `--prod` flag for a production build.