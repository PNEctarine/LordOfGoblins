using System;
using System.Collections;
using System.Collections.Generic;
using InternetCheck;
using InternetTime;
using Kuhpik;
using UnityEngine;

public class OfferSystem : GameSystemWithScreen<GameUI>
{
    private const string _offerKey = "OfferTime";

    public async override void OnInit()
    {
        screen.OfferButton.gameObject.SetActive(false);

        InternetAccessComponent.Instance.AsyncTestConnection(async result =>
        {
            if (result)
            {
                if (PlayerPrefs.HasKey("OfferTime"))
                {
                    //PlayerPrefs.SetString(_offerKey, $"{await InternetTimeManager.DateTimeCheck()}");

                    string time = PlayerPrefs.GetString("ExitTime", "00/00/00 00:00:00");

                    DateTime exitTime = Convert.ToDateTime(time);
                    DateTime currentTime = await InternetTimeManager.DateTimeCheck();

                    TimeSpan timeSpan = currentTime - exitTime;

                    if (timeSpan >= TimeSpan.FromDays(1))
                    {
                        if (player.IsOffer == true)
                        {
                            screen.OfferButton.gameObject.SetActive(false);
                            player.IsOffer = false;
                        }

                        else
                        {
                            screen.OfferButton.gameObject.SetActive(true);
                            player.IsOffer = true;
                        }

                        PlayerPrefs.SetString(_offerKey, $"{await InternetTimeManager.DateTimeCheck()}");
                    }
                }

                else
                {
                    PlayerPrefs.SetString(_offerKey, $"{await InternetTimeManager.DateTimeCheck()}");
                }
            }

            else
            {
                screen.OfferButton.gameObject.SetActive(false);
            }
        });
    }
}
