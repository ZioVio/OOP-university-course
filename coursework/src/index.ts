import Telegrah from 'telegraf';
import { config } from './config';

const bot = new Telegrah(config.BOT_TOKEN as string);

bot.launch().then(() => console.log('bot started')).catch((err: Error) => console.log('err', err));

bot.on('message', ctx => {
    ctx.reply(ctx.message?.text as string);  
});
