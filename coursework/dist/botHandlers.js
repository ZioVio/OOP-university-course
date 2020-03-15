"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
const index_1 = require("./index");
const telegraf_1 = __importDefault(require("telegraf"));
const UserStorage_1 = __importDefault(require("./storage/UserStorage"));
;
const bot = new telegraf_1.default();
bot.use((ctx, next) => {
    ctx.state.user = UserStorage_1.default.getUserById(ctx.message.chat.id);
    return next();
});
bot.command('start', (ctx) => {
    console.log(ctx.state.user);
    index_1.app.onStart(ctx);
});
exports.default = bot;