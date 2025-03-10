enum LogLevel {
	NONE = 0,
	WARN = 1,
	ERROR = 2
}

// TODO: send this logging procedure to the monorepo
export const logServer = (logLevel: LogLevel, message: string) => {
	let prefix;

	switch (logLevel) {
		case LogLevel.NONE:
			prefix = '>';
			break;
		case LogLevel.WARN:
			prefix = '?';
			break;
		case LogLevel.ERROR:
			prefix = '!';
			break;
	}

	console.log(`${prefix}: ${message}`);
};
