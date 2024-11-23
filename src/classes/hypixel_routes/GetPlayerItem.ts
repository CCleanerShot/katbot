export class GetPlayerItem {
	uuid: string; // "3fa85f6457174562b3fc2c963f66afa6
	displayname: string; // "string";
	rank: string; // "ADMIN";
	packageRank: string; // "MVP_PLUS";
	newPackageRank: string; // "MVP_PLUS";
	monthlyPackageRank: string; // "SUPERSTAR";
	firstLogin: number; // 0;
	lastLogin: number; // 0;
	lastLogout: number; // 0;
	stats: any; // {};

	constructor(params: {
		uuid: string;
		displayname: string;
		rank: string;
		packageRank: string;
		newPackageRank: string;
		monthlyPackageRank: string;
		firstLogin: number;
		lastLogin: number;
		lastLogout: number;
		stats: any;
	}) {
		this.uuid = params.uuid;
		this.displayname = params.displayname;
		this.rank = params.rank;
		this.packageRank = params.packageRank;
		this.newPackageRank = params.newPackageRank;
		this.monthlyPackageRank = params.monthlyPackageRank;
        this.firstLogin = params.firstLogin;
		this.lastLogin = params.lastLogin;
		this.lastLogout = params.lastLogout;
		this.stats = params.stats;
	}
}
