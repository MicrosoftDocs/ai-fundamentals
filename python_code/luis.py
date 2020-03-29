def get_intent(luis_app_id, cog_key, cog_endpoint, command):
    import json
    import requests

    action = 'unknown'

    try:
        # print the command to be interpreted
        print(command)

        # Set up the REST request
        headers = {
        }
        params ={
            'query': command,
            'subscription-key': cog_key
        }

        # Call the LUIS app and get the prediction
        response = requests.get(cog_endpoint + '/luis/prediction/v3.0/apps/'+ luis_app_id + '/slots/production/predict',
                                headers=headers, params=params)
        data = response.json()

        # Get the most probable intent
        intent = data["prediction"]["topIntent"]
        print('- predicted intent:',intent)

        if intent != 'None':
            # Get the target device
            entities = data["prediction"]["entities"]
            if 'device' in entities:
                # For simplicity, only the first 'device' entity is identified
                device = entities['device'][0][0]
                print('- predicted entity:',device)

                # Set the action to intent_device
                action = intent + '_' +  device

        return action

    except Exception as ex:
        print(ex)
        return 'unknown'
