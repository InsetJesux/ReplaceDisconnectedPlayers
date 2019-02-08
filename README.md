# Replacing Disconnected Players


Replace players who have been disconnected with spectators in the same position, health, items, ammo

## Configs
Config Option | Value Type | Default Value | Description
------------ | ------------- | ------------- | -------------
sod_allow_user_choice | Boolean | True | While disabled force setting replace for all users
sod_force_value | Boolean | False | Force this value for all players, if sod_allow_user_choice is false
sod_dropitems | Boolean | True | Drop items if there are no spectators available
sod_default_setting | Boolean | True | Default configuration for users who haven't used .sod command

## Client Commands
Command | Description
------------ | -------------
.sod enable | Enable user replace
.sod disable | Disable user replace
