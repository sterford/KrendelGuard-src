# KrendelGuard
# Приветствую! Это моя защита для майнкрафт клиентов.

В данном гите лежат исходники лоадера и сайта (frontend & backend)

К сожалению обфускатор я не предоставляю
Так-же хотел бы уточнить что я ставлю защиту платно, сливаю старую версию,
кто хочет купиь новую версию (не исходники защиты, а защиту на клиент) пишите в телеграмм
https://t.me/bludnov

## Пример обфусцированного кода
Ремапп + Флов обфускация + Переименовывание локальных переменных + обертывание в лямбда

Было:
```Java

@FunctionRegister(
    name = "Target Strafe",
    type = Category.Movement
)
public class TargetStrafe extends Function {
    private final SliderSetting distanceSetting = new SliderSetting("Дистанция", 1.0F, 0.1F, 6.0F, 0.05F);
    private final BooleanSetting damageBoostSetting = new BooleanSetting("Буст с дамагом", true);
    private final SliderSetting boostValueSetting = new SliderSetting("Значение буста", 1.5F, 0.1F, 5.0F, 0.05F);
    private final SliderSetting timeSetting = new SliderSetting("Время буста", 10.0F, 1.0F, 20.0F, 1.0F);
    private final BooleanSetting saveTarget = new BooleanSetting("Сохранять цель", true);
    private float side = 1.0F;
    private LivingEntity target = null;
    private final DamagePlayerUtil damageUtil = new DamagePlayerUtil();
    private String targetName = "";
    public StrafeMovement strafeMovement = new StrafeMovement();
    private final Aura killAura;

    public TargetStrafe(Aura killAura) {
        this.killAura = killAura;
        this.addSettings(new Setting[]{this.distanceSetting, this.damageBoostSetting, this.timeSetting, this.saveTarget});
    }

    @Subscribe
    private void onAction(ActionEvent e) {
        Minecraft var10000 = mc;
        if (Minecraft.player != null) {
            var10000 = mc;
            if (Minecraft.world != null) {
                this.handleEventAction(e);
                return;
            }
        }

    }

    @Subscribe
    public void onMotion(MovingEvent event) {
        Minecraft var10000 = mc;
        if (Minecraft.player != null) {
            var10000 = mc;
            if (Minecraft.world != null) {
                var10000 = mc;
                if (Minecraft.player.ticksExisted >= 10) {
                    boolean isLeftKeyPressed = InputMappings.isKeyDown(Minecraft.getInstance().getMainWindow().getHandle(), 65);
                    boolean isRightKeyPressed = InputMappings.isKeyDown(Minecraft.getInstance().getMainWindow().getHandle(), 68);
                    LivingEntity auraTarget = this.getTarget();
                    if (auraTarget != null) {
                        this.targetName = auraTarget.getName().getString();
                    }

                    if (this.shouldSaveTarget(auraTarget)) {
                        this.target = this.updateTarget(this.target);
                    } else {
                        this.target = auraTarget;
                    }

                    if (this.target != null && this.target.isAlive() && this.target.getHealth() > 0.0F) {
                        var10000 = mc;
                        if (Minecraft.player.collidedHorizontally) {
                            this.side *= -1.0F;
                        }

                        if (isLeftKeyPressed) {
                            this.side = 1.0F;
                        }

                        if (isRightKeyPressed) {
                            this.side = -1.0F;
                        }

                        var10000 = mc;
                        double var17 = Minecraft.player.getPosZ() - this.target.getPosZ();
                        Minecraft var10001 = mc;
                        double angle = Math.atan2(var17, Minecraft.player.getPosX() - this.target.getPosX());
                        double var16 = MoveUtils.getMotion();
                        Minecraft var10002 = mc;
                        angle += var16 / (double)Math.max(Minecraft.player.getDistance(this.target), this.distanceSetting.min) * (double)this.side;
                        double x = this.target.getPosX() + (double)(Float)this.distanceSetting.get() * Math.cos(angle);
                        double z = this.target.getPosZ() + (double)(Float)this.distanceSetting.get() * Math.sin(angle);
                        var10001 = mc;
                        double yaw = this.getYaw(Minecraft.player, x, z);
                        this.damageUtil.time(((Float)this.timeSetting.get()).longValue() * 100L);
                        float damageSpeed = (Float)this.boostValueSetting.get() / 10.0F;
                        double speed = this.strafeMovement.calculateSpeed(event, (Boolean)this.damageBoostSetting.get(), this.damageUtil.isNormalDamage(), true, damageSpeed);
                        event.getMotion().x = speed * -Math.sin(Math.toRadians(yaw));
                        event.getMotion().z = speed * Math.cos(Math.toRadians(yaw));
                    }

                    return;
                }
            }
        }
    }
```

Стало:
```Java

@۩ڞۢ٣ۡہۚۥۭ۱ۆځۜٳھ_KrendelGuard_۩ڞۢ٣ۡہۚۥۭ۱ۆځۜٳھ(
    name = "Target Strafe",
    type = ٽ۪ٻ۰ېؤُڕ٪ڻٹْڢۅپ_KrendelGuard_ٽ۪ٻ۰ېؤُڕ٪ڻٹْڢۅپ.Movement
)
public class ٠ڎ۪ڇڼڪۛٴۍڀڨځڱۍږ_KrendelGuard_٠ڎ۪ڇڼڪۛٴۍڀڨځڱۍږ extends ىڝ١ۘھٝڠڈٙ۳۔ٺِ٩ڥ_KrendelGuard_ىڝ١ۘھٝڠڈٙ۳۔ٺِ٩ڥ {
    private final ړں٦ډځٌ٘ګڗڦ۲٩ٺڿآ_KrendelGuard_ړں٦ډځٌ٘ګڗڦ۲٩ٺڿآ distanceSetting = new ړں٦ډځٌ٘ګڗڦ۲٩ٺڿآ_KrendelGuard_ړں٦ډځٌ٘ګڗڦ۲٩ٺڿآ("Дистанция", 1.0F, 0.1F, 6.0F, 0.05F);
    private final ۄۀۈږِڊڞګ٢ۘٺڜۣک۳_KrendelGuard_ۄۀۈږِڊڞګ٢ۘٺڜۣک۳ damageBoostSetting = new ۄۀۈږِڊڞګ٢ۘٺڜۣک۳_KrendelGuard_ۄۀۈږِڊڞګ٢ۘٺڜۣک۳("Буст с дамагом", true);
    private final ړں٦ډځٌ٘ګڗڦ۲٩ٺڿآ_KrendelGuard_ړں٦ډځٌ٘ګڗڦ۲٩ٺڿآ boostValueSetting = new ړں٦ډځٌ٘ګڗڦ۲٩ٺڿآ_KrendelGuard_ړں٦ډځٌ٘ګڗڦ۲٩ٺڿآ("Значение буста", 1.5F, 0.1F, 5.0F, 0.05F);
    private final ړں٦ډځٌ٘ګڗڦ۲٩ٺڿآ_KrendelGuard_ړں٦ډځٌ٘ګڗڦ۲٩ٺڿآ timeSetting = new ړں٦ډځٌ٘ګڗڦ۲٩ٺڿآ_KrendelGuard_ړں٦ډځٌ٘ګڗڦ۲٩ٺڿآ("Время буста", 10.0F, 1.0F, 20.0F, 1.0F);
    private final ۄۀۈږِڊڞګ٢ۘٺڜۣک۳_KrendelGuard_ۄۀۈږِڊڞګ٢ۘٺڜۣک۳ saveTarget = new ۄۀۈږِڊڞګ٢ۘٺڜۣک۳_KrendelGuard_ۄۀۈږِڊڞګ٢ۘٺڜۣک۳("Сохранять цель", true);
    private float side = 1.0F;
    private LivingEntity target = null;
    private final ٠٩گ۔ٰڅَۗڌۏُٖڐٰٛ_KrendelGuard_٠٩گ۔ٰڅَۗڌۏُٖڐٰٛ damageUtil = new ٠٩گ۔ٰڅَۗڌۏُٖڐٰٛ_KrendelGuard_٠٩گ۔ٰڅَۗڌۏُٖڐٰٛ();
    private String targetName = "";
    public آۛ٥ٵۋإۦۮۇڛڧ۪ۖٷۑ_KrendelGuard_آۛ٥ٵۋإۦۮۇڛڧ۪ۖٷۑ strafeMovement = new آۛ٥ٵۋإۦۮۇڛڧ۪ۖٷۑ_KrendelGuard_آۛ٥ٵۋإۦۮۇڛڧ۪ۖٷۑ();
    private final ٓڪڻےڵٚڗڈڐ٣ۑڌۙڔِ_KrendelGuard_ٓڪڻےڵٚڗڈڐ٣ۑڌۙڔِ killAura;

    public void KrendelGuard_ڽڊښڕ() {
        double var10000 = 6.273274137726281E-8 + 2.86678195190842E-7;
        var10000 = 209.02079377955084 * 831.7876165902009;
        var10000 = 3.7763186925678723E-7 - 320.4452706473023;
        var10000 = 1.28104189790332E-7 + 1.8258991839488093E-7;
        boolean var1 = false;
        boolean var10001 = true;

        while(true) {
            boolean var10002 = false;
            double var10003 = 3 + 128.3116249812437;
        }
    }

    public void KrendelGuard_ۦڮَٕ() {
        // $FF: Couldn't be decompiled
    }

    public void KrendelGuard_ڹۈګۡڛ٥ۛ٧چڕۊڒۍٗڹ_KrendelGuard_ڹۈګۡڛ٥ۛ٧چڕۊڒۍٗڹ() {
        // $FF: Couldn't be decompiled
    }

    public void KrendelGuard_٦ٺأْڑۡڬېے٩__ۨڦڇ_KrendelGuard_٦ٺأْڑۡڬېے٩__ۨڦڇ/* $FF was: KrendelGuard_٦ٺأْڑۡڬېے٩٬۩ۨڦڇ_KrendelGuard_٦ٺأْڑۡڬېے٩٬۩ۨڦڇ*/() {
        // $FF: Couldn't be decompiled
    }

    public _ڎ۪ڇڼڪۛٴۍڀڨځڱۍږ_KrendelGuard_٠ڎ۪ڇڼڪۛٴۍڀڨځڱۍږ/* $FF was: ٠ڎ۪ڇڼڪۛٴۍڀڨځڱۍږ_KrendelGuard_٠ڎ۪ڇڼڪۛٴۍڀڨځڱۍږ*/(ٓڪڻےڵٚڗڈڐ٣ۑڌۙڔِ_KrendelGuard_ٓڪڻےڵٚڗڈڐ٣ۑڌۙڔِ killAura) {
        this.killAura = killAura;
        this.addSettings(new ڦْڣڄڲڵٍټٕۦڬڱڡە٪_KrendelGuard_ڦْڣڄڲڵٍټٕۦڬڱڡە٪[]{this.distanceSetting, this.damageBoostSetting, this.timeSetting, this.saveTarget});
    }

    @Subscribe
    private void onAction(۬ڑڂّ۪ڻۨڅ۩ۇڱۭۣۧڴ_KrendelGuard_۬ڑڂّ۪ڻۨڅ۩ۇڱۭۣۧڴ e) {
        Minecraft var10000 = mc;
        if (Minecraft.player != null) {
            var10000 = mc;
            if (Minecraft.world != null) {
                this.handleEventAction(e);
                return;
            }
        }

    }

    @Subscribe
    public void onMotion(ڌٿڒۛگڣآ٣ٟڅۧٻأۑڔ_KrendelGuard_ڌٿڒۛگڣآ٣ٟڅۧٻأۑڔ event) {
        Minecraft var10000 = mc;
        if (Minecraft.player != null) {
            var10000 = mc;
            if (Minecraft.world != null) {
                var10000 = mc;
                if (Minecraft.player.ticksExisted >= 10) {
                    boolean isLeftKeyPressed = InputMappings.isKeyDown(Minecraft.getInstance().getMainWindow().getHandle(), 65);
                    boolean isRightKeyPressed = InputMappings.isKeyDown(Minecraft.getInstance().getMainWindow().getHandle(), 68);
                    LivingEntity auraTarget = this.getTarget();
                    if (auraTarget != null) {
                        this.targetName = auraTarget.getName().getString();
                    }

                    if (this.shouldSaveTarget(auraTarget)) {
                        this.target = this.updateTarget(this.target);
                    } else {
                        this.target = auraTarget;
                    }

                    if (this.target != null && this.target.isAlive() && this.target.getHealth() > 0.0F) {
                        var10000 = mc;
                        if (Minecraft.player.collidedHorizontally) {
                            this.side *= -1.0F;
                        }

                        if (isLeftKeyPressed) {
                            this.side = 1.0F;
                        }

                        if (isRightKeyPressed) {
                            this.side = -1.0F;
                        }

                        var10000 = mc;
                        double var17 = Minecraft.player.getPosZ() - this.target.getPosZ();
                        Minecraft var10001 = mc;
                        double angle = Math.atan2(var17, Minecraft.player.getPosX() - this.target.getPosX());
                        double var16 = ٛۘۢۖٓٗڼٛڽۘ٘ٞ۫ۈ۞_KrendelGuard_ٛۘۢۖٓٗڼٛڽۘ٘ٞ۫ۈ۞.getMotion();
                        Minecraft var10002 = mc;
                        angle += var16 / (double)Math.max(Minecraft.player.getDistance(this.target), this.distanceSetting.min) * (double)this.side;
                        double x = this.target.getPosX() + (double)(Float)this.distanceSetting.get() * Math.cos(angle);
                        double z = this.target.getPosZ() + (double)(Float)this.distanceSetting.get() * Math.sin(angle);
                        var10001 = mc;
                        double yaw = this.getYaw(Minecraft.player, x, z);
                        this.damageUtil.time(((Float)this.timeSetting.get()).longValue() * 100L);
                        float damageSpeed = (Float)this.boostValueSetting.get() / 10.0F;
                        double speed = this.strafeMovement.calculateSpeed(event, (Boolean)this.damageBoostSetting.get(), this.damageUtil.isNormalDamage(), true, damageSpeed);
                        event.getMotion().x = speed * -Math.sin(Math.toRadians(yaw));
                        event.getMotion().z = speed * Math.cos(Math.toRadians(yaw));
                    }

                    return;
                }
            }
        }

    }

```

## Информация
- bludnov (protect creator)
- NaksonPaster + Regu11ar (remapp helper)
- Crashdami (some ideas + help)
- Sertyo (help)
