using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// using Nethereum RPC
using Nethereum.JsonRpc.UnityClient;
// using contract definition
using Solidity.Contracts.Robot.ContractDefinition;

public class RobotContract : MonoBehaviour
{
    [Header("Environment Variables")]
    

    [Tooltip("Connect to ganache local network by default")]
    public string RpcUrl = "http://127.0.0.1:7545";

    [Tooltip("Set robot contract address after migrate")]
    public string robotContractAddress = "";

    [Tooltip("By default set your ganache wallet address to test")]
    public string walletAddress = "";


    [Header("Token ID's of Nfts in Wallet")]
    public List<int> robotList;

    [Header("Stats of all NFT's in Wallet")]
    public List<Attributes> robots;

    public bool isReady { get; private set; }

    private void Start()
    {
        isReady = false;
        StartCoroutine(Fetch());
    }

    private IEnumerator Fetch()
    {
        // connect to ganache local network by default
        string url = RpcUrl;
        // robot contract address
        string contractAddress = robotContractAddress;
        // Your wallet address
        string wallet = walletAddress;


        // fetch total supply
        var queryRequestSupply = new QueryUnityRequest<TotalSupplyFunctionBase, TotalSupplyOutputDTOBase>(url, contractAddress);
        // call TotalSupplyFunctionBase without params to get the robots total supply
        yield return queryRequestSupply.Query(new TotalSupplyFunctionBase(), contractAddress);
        // print in console
        print(queryRequestSupply.Result.totalSupply);


        // Get robot list of wallet address 0x886f01f7903D9E9c5E29Ca9B82ccBed9a46611D1
        var queryRequestRobotsList = new QueryUnityRequest<RobotsListFunctionBase, RobotsListOutputDTOBase>(url, contractAddress);
        var listFunction = new RobotsListFunctionBase() { From = wallet };
        yield return queryRequestRobotsList.Query(listFunction, contractAddress);
        print(queryRequestRobotsList.Result.robotsInWallet);

        foreach (var item in queryRequestRobotsList.Result.robotsInWallet)
        {
            robotList.Add((int)item);
        }


        // Get parts of all robots in wallet 
        foreach (var item in robotList)
        {
            var queryParts = new QueryUnityRequest<GetRobotPartsFunctionBase, GetRobotPartsOutputDTOBase>(url, contractAddress);

            yield return queryParts.Query(new GetRobotPartsFunctionBase() { TokenID = item }, contractAddress);

            robots.Add(
                new Attributes(
                    queryParts.Result.Head,
                    queryParts.Result.Body,
                    queryParts.Result.ArmsLeft,
                    queryParts.Result.ArmsRight,
                    queryParts.Result.Legs
                )
            );
        }

        isReady = true;
    }

    [System.Serializable]
    public struct Attributes
    {
        public Attributes(int head, int body, int armsLeft, int armsRight, int legs) {
            this.head = head;
            this.body = body;
            this.armsLeft = armsLeft;
            this.armsRight = armsRight;
            this.legs = legs;
        }

        public int head;
        public int body;
        public int armsLeft;
        public int armsRight;
        public int legs;
    }
}
