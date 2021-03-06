"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
const telegraf_1 = __importDefault(require("telegraf"));
const UserStorage_1 = __importDefault(require("./storage/UserStorage"));
const index_1 = require("./index");
const buttons_1 = __importDefault(require("./config/buttons"));
;
const bot = new telegraf_1.default();
bot.use((ctx, next) => {
    var _a, _b, _c, _d;
    const userId = ((_a = ctx === null || ctx === void 0 ? void 0 : ctx.message) === null || _a === void 0 ? void 0 : _a.chat.id) || ((_b = ctx.callbackQuery) === null || _b === void 0 ? void 0 : _b.message.chat.id);
    ctx.state.user = UserStorage_1.default.getUserById(userId);
    index_1.app.onAny(ctx);
    if (!((_d = (_c = ctx.state) === null || _c === void 0 ? void 0 : _c.user) === null || _d === void 0 ? void 0 : _d.state.isInFight()))
        return next();
});
bot.command('start', (ctx) => {
    if (!ctx.state.user)
        ctx.reply('Hello. Thanks for beginning!');
    index_1.app.onStart(ctx);
});
bot.hears(buttons_1.default.createNewFighter, (ctx) => {
    index_1.app.onCreateFighter(ctx);
});
bot.hears(buttons_1.default.showMyFighters, (ctx) => {
    index_1.app.onFightersShow(ctx);
});
bot.hears([...Object.values(buttons_1.default.fighters)], (ctx) => {
    index_1.app.onFighterTypeSelected(ctx);
});
bot.hears(buttons_1.default.showEnemies, (ctx) => {
    index_1.app.onEmeniesShow(ctx);
});
bot.hears(buttons_1.default.startFight, (ctx) => {
    index_1.app.onFight(ctx);
});
bot.on('text', (ctx) => {
    index_1.app.onText(ctx);
});
bot.on('callback_query', (ctx) => {
    index_1.app.onCallbackQuery(ctx);
});
exports.default = bot;
