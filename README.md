## Table of contents
* [General info](#general-info)
* [Technologies](#technologies)
* [Setup](#setup)
* [Assumptions](#assumptions)
* [ToDo](#todo-list)

## General info
This project is created by Danny.Yu for MegaMerge puzzle
	
## Technologies
Project is written in C# with .net core 3.1

## Setup
To run this project in Windows, 
```
Unzip the release.zip file
Put the input files in \input and config the appSettings.json to make sure input files are in the inputConfig
Click the executable file: MegaMerge.exe
Found the output file in \output
```

Edit the appSettings.json to change the configuration, see sample appSettings file above.
inputConfig structure:


```
-- FolderPath
-- FileDictionary
	-- CompanyCode: 1
		- FileType
			-- Filenames

```


To extend more company, add here: 
```
	-- CompanyCode: n
		- FileType
			-- Filenames
```
</pre>

output folder(can be configured in appSettings.json):
```
\output
```

## Assumptions
1. We only have barcode, catalog and supplier, but if we want to extend this, it will not be a good place to add new types of source.
2. All catalog and suppliers are presented in barcode input file
## ToDo
1. Replace the DataRepository with proper and different database repositories for different entities.
2. More unit tests.
3. Merge 3 ConsumeServices into 1 generic service.
