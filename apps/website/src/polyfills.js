// @ts-nocheck
BigInt.prototype['toJSON'] = function () { 
    return this.toString()
}