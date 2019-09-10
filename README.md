# UciRod.Inventapp
This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 8.0.3.

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

* implement await/async
* implement auditing
* implement mailing
* implement reporting
* implement caching
* implement authentication use JWT to authenticate
* add auth to swagger
* use hangfire
* use IIS
* use Jenkins/TeamCity
* implement integration events with NServiceBus
* implement at least one soap service
* implement import using Excel library
* create application to show logs (maybe will be necessary to migrate to a database)
* implment process to delete old logs (one mounth) folder (maybe directly in LogService)
* unit test


## Angular architercure styleguide

https://angular.io/guide/styleguide

## Development server

Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files.

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

## Build

Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory. Use the `--prod` flag for a production build.