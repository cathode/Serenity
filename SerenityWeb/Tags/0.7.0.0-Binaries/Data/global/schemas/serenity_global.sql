CREATE TABLE [domains] (
[id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
[name] VARCHAR(160)  UNIQUE NOT NULL,
[is_enabled] BOOLEAN DEFAULT 'TRUE' NOT NULL,
[document_root] VARCHAR(128)  NULL,
[theme_name] VARCHAR(32)  NULL
);

CREATE TABLE [logs] (
[id] INTEGER  PRIMARY KEY AUTOINCREMENT NOT NULL,
[level] INTEGER DEFAULT '0' NULL,
[time] TIMESTAMP DEFAULT CURRENT_TIMESTAMP NULL,
[message] TEXT  NULL,
[trace] TEXT  NULL
);

CREATE TABLE [modules] (
[name] VARCHAR(32)  UNIQUE NOT NULL PRIMARY KEY,
[assembly_path] VARCHAR(240)  NOT NULL,
[is_enabled] BOOLEAN DEFAULT 'TRUE' NOT NULL
);

CREATE TABLE [routes] (
[path] VARCHAR(128)  UNIQUE NOT NULL,
[hostname] VARCHAR(128)  NULL,
[target_url] VARCHAR(255)  NOT NULL,
[is_enabled] BOOLEAN DEFAULT 'TRUE' NOT NULL
);

CREATE TABLE [server_configs] (
[id] INTEGER DEFAULT '0' NOT NULL PRIMARY KEY AUTOINCREMENT,
[port] INTEGER DEFAULT '80' NOT NULL,
[log_to_file] BOOLEAN DEFAULT 'TRUE' NOT NULL,
[log_to_console] BOOLEAN DEFAULT 'TRUE' NOT NULL
);

CREATE TABLE [session_data] (
[id] CHAR(32)  NOT NULL,
[name] VARCHAR(32)  NOT NULL,
[value] TEXT  NULL
);

CREATE TABLE [sessions] (
[id] CHAR(32)  UNIQUE NOT NULL,
[created] TIMESTAMP DEFAULT CURRENT_TIMESTAMP NULL,
[lifetime] INTEGER DEFAULT '300000' NULL,
[last_modified] TIMESTAMP DEFAULT CURRENT_TIMESTAMP NULL
);

