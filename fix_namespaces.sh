#!/bin/bash

# Fix CardLayout namespace conflicts
sed -i '4a using CardLayoutEntity = Commitments.Core.Model.CardLayoutAggregate.CardLayout;' "src/Commitments.Api/Features/CardLayout/CardLayoutExtensions.cs"
sed -i 's/this CardLayout cardLayout/this CardLayoutEntity cardLayout/g' "src/Commitments.Api/Features/CardLayout/CardLayoutExtensions.cs"
sed -i 's/IQueryable<CardLayout> cardLayouts/IQueryable<CardLayoutEntity> cardLayouts/g' "src/Commitments.Api/Features/CardLayout/CardLayoutExtensions.cs"

# Fix Dashboard namespace conflicts
sed -i '4a using DashboardEntity = Commitments.Core.Model.DashboardAggregate.Dashboard;' "src/Commitments.Api/Features/Dashboard/DashboardExtensions.cs"
sed -i 's/this Dashboard dashboard/this DashboardEntity dashboard/g' "src/Commitments.Api/Features/Dashboard/DashboardExtensions.cs"
sed -i 's/IQueryable<Dashboard> dashboards/IQueryable<DashboardEntity> dashboards/g' "src/Commitments.Api/Features/Dashboard/DashboardExtensions.cs"

# Fix DashboardCard namespace conflicts
sed -i '4a using DashboardCardEntity = Commitments.Core.Model.DashboardCardAggregate.DashboardCard;' "src/Commitments.Api/Features/DashboardCard/DashboardCardExtensions.cs"
sed -i 's/this DashboardCard dashboardCard/this DashboardCardEntity dashboardCard/g' "src/Commitments.Api/Features/DashboardCard/DashboardCardExtensions.cs"
sed -i 's/IQueryable<DashboardCard> dashboardCards/IQueryable<DashboardCardEntity> dashboardCards/g' "src/Commitments.Api/Features/DashboardCard/DashboardCardExtensions.cs"
sed -i 's/List<DashboardCard> DashboardCards/List<DashboardCardEntity> DashboardCards/g' "src/Commitments.Api/Features/Dashboard/Commands/CreateDashboard.cs"

# Fix DigitalAsset namespace conflicts
sed -i '4a using DigitalAssetEntity = Commitments.Core.Model.DigitalAssetAggregate.DigitalAsset;' "src/Commitments.Api/Features/DigitalAsset/DigitalAssetDto.cs"
sed -i 's/FromDigitalAsset(DigitalAsset entity)/FromDigitalAsset(DigitalAssetEntity entity)/g' "src/Commitments.Api/Features/DigitalAsset/DigitalAssetDto.cs"

# Fix Frequency namespace conflicts
sed -i '5a using FrequencyEntity = Commitments.Core.Model.FrequencyAggregate.Frequency;' "src/Commitments.Api/Features/Frequency/FrequencyDto.cs"
sed -i 's/FromFrequency(Frequency frequency)/FromFrequency(FrequencyEntity frequency)/g' "src/Commitments.Api/Features/Frequency/FrequencyDto.cs"

# Fix FrequencyType namespace conflicts
sed -i '5a using FrequencyTypeEntity = Commitments.Core.Model.FrequencyTypeAggregate.FrequencyType;' "src/Commitments.Api/Features/FrequencyType/FrequencyTypeDto.cs"
sed -i 's/FromFrequencyType(FrequencyType frequencyType)/FromFrequencyType(FrequencyTypeEntity frequencyType)/g' "src/Commitments.Api/Features/FrequencyType/FrequencyTypeDto.cs"

echo "Namespace conflicts fixed"
